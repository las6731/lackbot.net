import { Component, Injector } from '@angular/core';
import { TuiDialogService } from '@taiga-ui/core';
import { first } from 'rxjs';
import * as util from 'src/util/util';
import { AutoReact, AutoReactType } from '../../models/autoreact.model';
import { ReactFilter } from '../../models/reactfilter.model';
import { ReactsService } from '../../services/reacts.service';
import { PolymorpheusComponent } from '@tinkoff/ng-polymorpheus';
import { ReactDialogComponent } from '../react-dialog/react-dialog.component';

@Component({
  selector: 'app-reacts',
  templateUrl: './reacts.component.html',
  styleUrls: ['./reacts.component.scss']
})
export class ReactsComponent {

  typeOptions = Object.keys(AutoReactType).map(type => type as AutoReactType);
  filter: ReactFilter = new ReactFilter();
  reacts: AutoReact[];
  loaded = false;

  constructor(private service: ReactsService, private dialogService: TuiDialogService, private injector: Injector) {
    this.reacts = [];
    service.$reacts.pipe(first()).subscribe(_ => this.loaded = true);
    service.$reacts.subscribe(reacts => this.reacts = reacts);
    this.service.getReacts();
  }

  public typeForDisplay(type: AutoReactType): string {
    return util.reactTypeForDisplay(type);
  }

  get filteredReactions(): AutoReact[] {
    return this.reacts
      .filter(react => this.filter.types.length == 0 || this.filter.types.includes(react.type))
      .filter(react => this.filter.description == '' ||
        react.description.includes(this.filter.description) ||
        react.phrase.includes(this.filter.description));
  }

  get selectContent(): string {
    let str = this.filter.types.map(type => util.reactTypeForDisplay(type)).join(', ');
    return str.length > 21 ? str.substring(0, 21) + '...' : str;
  }

  clearFilters(): void {
    this.filter = new ReactFilter();
  }

  public openDialog(): void {
    this.dialogService.open(new PolymorpheusComponent(ReactDialogComponent, this.injector), { size: 'm' }).subscribe();
  }

}
