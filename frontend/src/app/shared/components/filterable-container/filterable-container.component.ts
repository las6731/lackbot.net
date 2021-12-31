import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-filterable-container',
  templateUrl: './filterable-container.component.html',
  styleUrls: ['./filterable-container.component.scss']
})
export class FilterableContainerComponent {

  @Input() loaded: boolean = true;

}
