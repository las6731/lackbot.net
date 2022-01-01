import { AutoReactType } from "src/app/reacts/models/autoreact.model";
import { AutoResponseType } from "src/app/responses/models/autoresponse.model";

export function responseTypeForDisplay(type: AutoResponseType): string {
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

export function reactTypeForDisplay(type: AutoReactType): string {
  switch (type) {
    case AutoReactType.Author:
      return 'Author';
    case AutoReactType.Strong:
      return 'Strong';
    default:
      return 'Weak';
  }
}

export function isTimeBased(type: AutoResponseType): boolean {
    return type == AutoResponseType.TimeBased || type == AutoResponseType.TimeBasedYesNo;
}
