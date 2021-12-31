import { Component, Injector, Input } from '@angular/core';
import { AutoResponse, AutoResponseType } from '../models/autoresponse.model';
import * as util from 'src/util/util';
import { TuiDialogService } from '@taiga-ui/core';
import { PolymorpheusComponent } from '@tinkoff/ng-polymorpheus';
import { faClock, faCode, faDumbbell } from '@fortawesome/free-solid-svg-icons';
import { DialogComponent } from '../dialog/dialog.component';

@Component({
  selector: 'app-response',
  templateUrl: './response.component.html',
  styleUrls: ['./response.component.scss']
})
export class ResponseComponent {

  @Input() response!: AutoResponse

  strongIcon = faDumbbell;
  timeIcon = faClock;
  regexIcon = faCode;

  constructor(private dialogService: TuiDialogService, private injector: Injector) { }

  get typeDisplay(): string {
    return util.forDisplay(this.response.type);
  }

  get responseDisplay(): string {
    if (this.response.responses.length == 1) {
      if (this.response.responses[0].length < 28) return this.response.responses[0];
      return this.response.responses[0].substring(0, 26) + '...';
    }
    return `${this.response.responses.length} responses`;
  }

  get isTimeBased(): boolean {
    return util.isTimeBased(this.response.type);
  }

  get isRegex(): boolean {
    return this.response.type == AutoResponseType.Regex;
  }

  get isStrong(): boolean {
    return this.response.type == AutoResponseType.Strong;
  }

  public typeForDisplay(type: AutoResponseType): string {
    return util.forDisplay(type);
  }

  public openDialog(): void {
    this.dialogService.open(new PolymorpheusComponent(DialogComponent, this.injector), {
      size: 'm',
      data: this.response
    }).subscribe();
  }
}
