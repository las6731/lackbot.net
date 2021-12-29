import { Component, OnInit } from '@angular/core';
import { first } from 'rxjs';
import { AutoResponse, AutoResponseType } from './models/autoresponse.model';
import { ResponsesService } from './services/responses.service';

@Component({
  selector: 'app-responses',
  templateUrl: './responses.component.html',
  styleUrls: ['./responses.component.scss']
})
export class ResponsesComponent implements OnInit {

  typeOptions = Object.keys(AutoResponseType).map(type => type as AutoResponseType);
  filter: ResponseFilter = new ResponseFilter();
  responses: AutoResponse[];
  loaded = false;

  constructor(private service: ResponsesService) {
    this.responses = [];
    service.$responses.pipe(first()).subscribe(_ => this.loaded = true);
    service.$responses.subscribe(responses => this.responses = responses);
    this.service.getResponses();
  }

  ngOnInit(): void {
    
  }

  typeForDisplay(type: AutoResponseType) : string {
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

  get filteredResponses(): AutoResponse[] {
    return this.responses.filter(response => {
      return (this.filter.description == '' || response.description.includes(this.filter.description)) &&
      (this.filter.types.length == 0 || this.filter.types.includes(response.type))
    });
  }

  clearFilters(): void {
    this.filter = new ResponseFilter();
  }

}

class ResponseFilter {
  public description: string = '';
  public types: AutoResponseType[] = [];
}