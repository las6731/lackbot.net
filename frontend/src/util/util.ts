import { AutoResponseType } from "src/app/responses/models/autoresponse.model";

export function forDisplay(type: AutoResponseType): string {
    switch (type) {
        case AutoResponseType.TimeBasedYesNo:
          return 'Time-Based Yes/No';
        case AutoResponseType.TimeBased:
          return 'Time-Based';
        case AutoResponseType.Regex:
          return 'Regex';
        case AutoResponseType.Strong:
          return 'Strong';
        default:
          return 'Weak';
    }
}

export function isTimeBased(type: AutoResponseType): boolean {
    return type == AutoResponseType.TimeBased || type == AutoResponseType.TimeBasedYesNo;
}
