import { Component, signal, OnInit, OnDestroy, computed } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { SignalRMethods } from './signalr-constants';

export interface RoleDataResponse {
  Role: string;
  Message: string;
}

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [CommonModule, FormsModule],
  template: `
    <div>
      <h1>Global Timer</h1>
      <p>Status: {{ isTimeConnected() ? 'Live' : 'Off' }}</p>
      <h2>{{ formattedTime() }}</h2>
      <p>Hub: /timehub</p>

      <hr />

      <h1>Role Specific Data</h1>
      <p>Status: {{ isRoleConnected() ? 'Live' : 'Off' }}</p>
      
      <div>
        <label>Role: </label>
        <select [(ngModel)]="selectedRole" (change)="onRoleChange()">
          <option value="Guest">Guest</option>
          <option value="Admin">Admin</option>
          <option value="Manager">Manager</option>
          <option value="User">User</option>
        </select>
      </div>

      <div *ngIf="roleData(); else loading">
        <h3>Data for: {{ selectedRole() }}</h3>
        <pre>{{ roleData() | json }}</pre>
        <p>Message: {{ roleData()?.Message }}</p>
      </div>
      <ng-template #loading>
        <p>Connecting...</p>
      </ng-template>
    </div>
  `,
})
export class App implements OnInit, OnDestroy {
  private readonly baseUrl = 'https://localhost:7033';
  
  // TimeHub State
  protected readonly currentTime = signal<string | null>(null);
  protected readonly isTimeConnected = signal(false);
  protected readonly formattedTime = computed(() => {
    const time = this.currentTime();
    return time ? new Date(time).toLocaleTimeString() : '--:--:--';
  });

  // RoleHub State
  protected readonly selectedRole = signal('Guest');
  protected readonly roleData = signal<RoleDataResponse | null>(null);
  protected readonly isRoleConnected = signal(false);

  private timeHub: signalR.HubConnection | undefined;
  private roleHub: signalR.HubConnection | undefined;

  async ngOnInit() {
    await this.connectTimeHub();
    await this.connectRoleHub();
  }

  private async connectTimeHub() {
    this.timeHub = new signalR.HubConnectionBuilder()
      .withUrl(`${this.baseUrl}/timehub`)
      .withAutomaticReconnect()
      .build();

    this.timeHub.on(SignalRMethods.TimeHub.ReceiveTime, (time: string) => this.currentTime.set(time));
    this.timeHub.onreconnecting(() => this.isTimeConnected.set(false));
    this.timeHub.onreconnected(() => this.isTimeConnected.set(true));

    try {
      await this.timeHub.start();
      this.isTimeConnected.set(true);
    } catch (err) {
      console.error('TimeHub Error:', err);
    }
  }

  private async connectRoleHub() {
    if (this.roleHub) {
      await this.roleHub.stop();
    }

    this.roleHub = new signalR.HubConnectionBuilder()
      .withUrl(`${this.baseUrl}/rolehub?role=${this.selectedRole()}`)
      .withAutomaticReconnect()
      .build();

    this.roleHub.on(SignalRMethods.RoleHub.ReceiveRoleData, (data: RoleDataResponse) => this.roleData.set(data));
    this.roleHub.onreconnecting(() => this.isRoleConnected.set(false));
    this.roleHub.onreconnected(() => this.isRoleConnected.set(true));

    try {
      await this.roleHub.start();
      this.isRoleConnected.set(true);
    } catch (err) {
      console.error('RoleHub Error:', err);
    }
  }

  async onRoleChange() {
    this.roleData.set(null);
    await this.connectRoleHub();
  }

  ngOnDestroy() {
    this.timeHub?.stop();
    this.roleHub?.stop();
  }
}
