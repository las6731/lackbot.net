import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';
import { environment } from 'src/environments/environment';
import { ScheduledMessage } from '../models/scheduled-message.model';

@Injectable({
  providedIn: 'root'
})
export class MessagesService {

  $messages: Subject<ScheduledMessage[]>;

  constructor(private httpClient: HttpClient) {
    this.$messages = new Subject();
  }

  public getMessages(): void {
    this.httpClient.get(`${environment.API_URL}/scheduled-messages`).subscribe((res: any) => {
      let messages = res as ScheduledMessage[];
      messages.forEach(msg => msg.description = msg.timeSchedule);
      this.$messages.next(messages);
    });
  }

  public addScheduledMessage(message: ScheduledMessage): void {
    let command = message as any;
    command.channelId = message.channelId.toString();
    this.httpClient.post(`${environment.API_URL}/scheduled-messages`, command)
      .subscribe(() => this.getMessages());
  }

  public updateScheduledMessage(message: ScheduledMessage): void {
    let command = message as any;
    command.channelId = message.channelId.toString();
    this.httpClient.put(`${environment.API_URL}/scheduled-messages/${message.id}`, command)
      .subscribe(() => this.getMessages());
  }

  public deleteScheduledMessage(id: string): void {
    this.httpClient.delete(`${environment.API_URL}/scheduled-messages/${id}`)
      .subscribe(() => this.getMessages());
  }
  
}
