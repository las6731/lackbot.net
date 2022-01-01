import { Component, Injector, Input } from '@angular/core';
import { faDumbbell, faUser } from '@fortawesome/free-solid-svg-icons';
import { TuiDialogService } from '@taiga-ui/core';
import * as util from 'src/util/util';
import { AutoReact, AutoReactType } from '../../models/autoreact.model';
import { PolymorpheusComponent } from '@tinkoff/ng-polymorpheus';
import { ReactDialogComponent } from '../react-dialog/react-dialog.component';

@Component({
  selector: 'app-react',
  templateUrl: './react.component.html',
  styleUrls: ['./react.component.scss']
})
export class ReactComponent {

  @Input() react!: AutoReact

  strongIcon = faDumbbell;
  authorIcon = faUser;

  constructor(private dialogService: TuiDialogService, private injector: Injector) { }

  get typeDisplay(): string {
    return util.reactTypeForDisplay(this.react.type);
  }

  get isAuthor(): boolean {
    return this.react.type == AutoReactType.Author;
  }

  get isStrong(): boolean {
    return this.react.type == AutoReactType.Strong;
  }

  public typeForDisplay(type: AutoReactType): string {
    return util.reactTypeForDisplay(type);
  }

  public openDialog(): void {
    this.dialogService.open(new PolymorpheusComponent(ReactDialogComponent, this.injector), {
      size: 'm',
      data: this.react
    }).subscribe();
  }

}
