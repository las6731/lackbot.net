export class ScheduledMessage {

    public id: string;

    public channelId: number;

    public timeSchedule: string;

    public messages: string[];

    public constructor(channelId: number, timeSchedule: string, messages: string[]) {
        this.channelId = channelId;
        this.timeSchedule = timeSchedule;
        this.messages = messages;
    }
}
