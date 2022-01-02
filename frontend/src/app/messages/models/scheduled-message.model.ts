export class ScheduledMessage {

    public id: string | undefined;
    public channelId: bigint;
    public description: string;
    public timeSchedule: string;
    public messages: string[];

    public constructor(id: string, channelId: bigint, description: string, timeSchedule: string, messages: string[]) {
        this.id = id;
        this.channelId = channelId;
        this.description = description;
        this.timeSchedule = timeSchedule;
        this.messages = messages;
    }
}
