import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { Subject } from 'rxjs';
import { ScheduledMessage } from '../../models/scheduledmessage.model';

@Injectable({
    providedIn: 'root'
})
export class ScheduledMessagesService {

    $messages: Subject<ScheduledMessage[]>;

    constructor(private httpClient: HttpClient) {
        this.$messages = new Subject();
    }

    public getMessages(): void {
        this.httpClient.get(`${environment.API_URL}/scheduled-messages`).subscribe((messages: ScheduledMessage[]) => {
            this.$messages.next(messages);
        });
    }

    public addScheduledMessage(message: ScheduledMessage): void {
        this.httpClient.post(`${environment.API_URL}/scheduled-messages`, message)
            .subscribe(() => this.getMessages());
    }

    public addMessage(id: string, message: string): void {
        const headers = new HttpHeaders().set('Content-Type', 'application/json; charset=utf-8');
        this.httpClient.post(`${environment.API_URL}/scheduled-messages/${id}`, `"${message}"`, { headers: headers })
            .subscribe(() => this.getMessages());
    }

    public updateScheduledMessage(message: ScheduledMessage): void {
        this.httpClient.put(`${environment.API_URL}/scheduled-messages/${message.id}`, message)
            .subscribe(() => this.getMessages());
    }

    public updateMessage(id: string, index: number, message: string): void {
        const headers = new HttpHeaders().set('Content-Type', 'application/json; charset=utf-8');
        this.httpClient.put(`${environment.API_URL}/scheduled-messages/${id}/${index}`, `"${message}"`, { headers: headers })
            .subscribe(() => this.getMessages());
    }

    public deleteScheduledMessage(id: string): void {
        this.httpClient.delete(`${environment.API_URL}/scheduled-messages/${id}`)
            .subscribe(() => this.getMessages());
    }

    public deleteMessage(id: string, index: number): void {
        this.httpClient.delete(`${environment.API_URL}/scheduled-messages/${id}/${index}`)
            .subscribe(() => this.getMessages());
    }
}
