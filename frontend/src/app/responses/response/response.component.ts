import { Component, Input, OnInit } from '@angular/core';
import { AutoResponse } from '../models/autoresponse.model';

@Component({
  selector: 'app-response',
  templateUrl: './response.component.html',
  styleUrls: ['./response.component.scss']
})
export class ResponseComponent implements OnInit {

  @Input() response!: AutoResponse

  constructor() { }

  ngOnInit(): void {
  }

  public openDialog() {
    // todo
  }

}
