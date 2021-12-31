import { Component, Injector, OnInit } from '@angular/core';
import { TuiDialogService } from '@taiga-ui/core';
import { first } from 'rxjs';
import * as util from 'src/util/util';
import { DialogComponent } from './dialog/dialog.component';
import { AutoResponse, AutoResponseType } from './models/autoresponse.model';
import { ResponseFilter } from './models/responsefilter.model';
import { ResponsesService } from './services/responses.service';
import { PolymorpheusComponent } from '@tinkoff/ng-polymorpheus';


@Component({
  selector: 'app-responses',
  templateUrl: './responses.component.html',
  styleUrls: ['./responses.component.scss']
})
export class ResponsesComponent {

  typeOptions = Object.keys(AutoResponseType).map(type => type as AutoResponseType);
  filter: ResponseFilter = new ResponseFilter();
  responses: AutoResponse[];
  loaded = false;

  constructor(private service: ResponsesService, private dialogService: TuiDialogService, private injector: Injector) {
    this.responses = [];
    service.$responses.pipe(first()).subscribe(_ => this.loaded = true);
    service.$responses.subscribe(responses => this.responses = responses);
    this.service.getResponses();
  }

  public typeForDisplay(type: AutoResponseType): string {
    return util.forDisplay(type);
  }

  get filteredResponses(): AutoResponse[] {
    return this.responses
      .filter(response => this.filter.types.length == 0 || this.filter.types.includes(response.type))
      .filter(response => this.filter.description == '' ||
        response.description.includes(this.filter.description) ||
        response.phrase.includes(this.filter.description) ||
        response.responses.some(res => res.includes(this.filter.description)));
  }

  get selectContent(): string {
    let str = this.filter.types.map(type => util.forDisplay(type)).join(', ');
    return str.length > 21 ? str.substring(0, 21) + '...' : str;
  }

  clearFilters(): void {
    this.filter = new ResponseFilter();
  }

  public openDialog(): void {
    this.dialogService.open(new PolymorpheusComponent(DialogComponent, this.injector), { size: 'm' }).subscribe();
  }

}
