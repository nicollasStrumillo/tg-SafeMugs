import { Injectable, inject } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { DesafioResponse } from '../../pages/score-board/score-board.models';
import { Subject } from 'rxjs';
import { environment } from '../../../environments/environment';
import { AuthSessionService } from '../usuario/auth/auth-session.service';

@Injectable({
	providedIn: 'root',
})
export class SignalRService {
	private readonly authSessionService = inject(AuthSessionService);

	private readonly desafioSolvedSubject = new Subject<DesafioResponse>();
	public readonly desafioSolved$ = this.desafioSolvedSubject.asObservable();

	private connection!: signalR.HubConnection;

	public async startSignalRConnection(): Promise<void> {
		if (this.connection && this.connection.state === signalR.HubConnectionState.Connected) {
			console.log("A conexão SignalR já foi estabelecida.");
			return;
		}

		const token = this.authSessionService.token();
		const url = token
			? `${environment.signalRUrl}?access_token=${token}`
			: environment.signalRUrl;

		this.connection = new signalR.HubConnectionBuilder()
			.withUrl(url)
			.withAutomaticReconnect()
			.build();

		this.connection.on("DesafioSolved", (desafio: DesafioResponse) => {
			this.desafioSolvedSubject.next(desafio);
		});

		await this.connection
			.start()
			.then(() => console.log("Connected to SignalR Hub"))
			.catch((err) => console.log("SignalR connection error:", err));
	}

	public async aknowledgeNotification(desafioId: number): Promise<void> {
		if (this.connection && this.connection.state === signalR.HubConnectionState.Connected) {
			await this.connection.invoke("AcknowledgeNotification", desafioId)
				.catch((err) => console.error("Error acknowledging notification:", err));
		} else {
			console.warn("SignalR connection is not established. Cannot acknowledge notification.");
			return Promise.reject("SignalR connection is not established.");
		}
	}

	public async SolveDesafioDomXss(payload: string): Promise<void> {
		if (this.connection && this.connection.state === signalR.HubConnectionState.Connected) {
			await this.connection.invoke("SolveDesafioDomXss", payload)
				.catch((err) => console.error("Error invoking SolveDesafioDomXss:", err));
		} else {
			console.warn("SignalR connection is not established. Cannot invoke SolveDesafioDomXss.");
			return Promise.reject("SignalR connection is not established.");
		}
	}

	public async SolveDesafioStoredXss(payload: string): Promise<void> {
		if (this.connection && this.connection.state === signalR.HubConnectionState.Connected) {
			await this.connection.invoke("SolveDesafioStoredXss", payload)
				.catch((err) => console.error("Error invoking SolveDesafioStoredXss:", err));
		} else {
			console.warn("SignalR connection is not established. Cannot invoke SolveDesafioStoredXss.");
			return Promise.reject("SignalR connection is not established.");
		}
	}
}
