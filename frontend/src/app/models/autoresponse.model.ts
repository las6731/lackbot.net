export class AutoResponse {

    public id: string;

    public phrase: string;

    public responses: string[];

    public timeSchedule: string;

    public type: AutoResponseType;

    public constructor(phrase: string, responses: string[]) {
        this.phrase = phrase;
        this.responses = responses;
        this.type = AutoResponseType.Naive;
    }

    public static Parse(apiResponse: any): AutoResponse {
        let response = new AutoResponse(apiResponse.phrase, apiResponse.responses);
        response.id = apiResponse.id;

        let type: string = apiResponse.$type;
        if (type.includes(AutoResponseType.Regex)) {
            response.type = AutoResponseType.Regex;
        } else if (type.includes(AutoResponseType.TimeBasedYesNo)) {
            response.type = AutoResponseType.TimeBasedYesNo;
            response.timeSchedule = apiResponse.timeSchedule;
        } else if (type.includes(AutoResponseType.TimeBased)) {
            response.type = AutoResponseType.TimeBased;
            response.timeSchedule = apiResponse.timeSchedule;
        } else if (type.includes(AutoResponseType.Strong)) {
            response.type = AutoResponseType.Strong;
        } else {
            response.type = AutoResponseType.Naive;
        }

        return response;
    }
}

export enum AutoResponseType {
    Naive = "Naive",
    Strong = "Strong",
    TimeBased = "TimeBased",
    TimeBasedYesNo = "TimeBasedYesNo",
    Regex = "Regex"
}
