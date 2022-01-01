import { ChangeDetectorRef, Component, Inject, Injector } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { faDumbbell, faUser } from '@fortawesome/free-solid-svg-icons';
import * as util from 'src/util/util';
import { TuiDialogContext, TuiDialogService } from '@taiga-ui/core';
import { POLYMORPHEUS_CONTEXT, PolymorpheusComponent } from '@tinkoff/ng-polymorpheus';
import { first, firstValueFrom } from 'rxjs';
import { ConfirmationDialogComponent } from 'src/app/shared/components/confirmation-dialog/confirmation-dialog.component';
import { AutoReact, AutoReactType } from '../../models/autoreact.model';
import { ReactsService } from '../../services/reacts.service';

@Component({
  selector: 'app-react-dialog',
  templateUrl: './react-dialog.component.html',
  styleUrls: ['./react-dialog.component.scss']
})
export class ReactDialogComponent {

  strongIcon = faDumbbell;
  authorIcon = faUser;

  typeOptions = Object.keys(AutoReactType).map(type => type as AutoReactType);

  form: FormGroup;
  value: AutoReact | null = null;
  isEdit = true;
  loading = false;
  deleting = false;

  constructor(private fb: FormBuilder, private service: ReactsService, private dialogService: TuiDialogService, private injector: Injector, private changeDetector: ChangeDetectorRef,
    @Inject(POLYMORPHEUS_CONTEXT) public context: TuiDialogContext<AutoReact | null, AutoReact | null>) {
    if (this.context.data == null) {
      this.isEdit = false;
    }

    this.form = fb.group({
      description: fb.control({ value: this.context.data?.description ?? '', disabled: true}),
      phrase: fb.control(this.context.data?.phrase ?? ''),
      type: fb.control(this.context.data?.type ?? AutoReactType.Naive, Validators.required),
      emoji: fb.control(this.context.data?.emoji ?? '', Validators.required),
      author: fb.control(this.context.data?.author ?? '')
    });

    if (this.isEdit) this.form.disable();
  }

  public typeForDisplay(type: AutoReactType): string {
    return util.reactTypeForDisplay(type);
  }

  get isAuthor(): boolean {
    return this.form.controls['type'].value == AutoReactType.Author;
  }

  get isStrong(): boolean {
    return this.form.controls['type'].value == AutoReactType.Strong;
  }

  get canSave(): boolean {
    return this.form.valid && this.form.dirty && this.form.enabled;
  }

  public edit(): void {
    if (this.form.disabled) this.form.enable();
  }

  public save(): void {
    this.form.disable();
    this.loading = true;

    this.service.$reacts.pipe(first()).subscribe(() => {
      this.loading = false;
      if (!this.isEdit) this.context.completeWith(null);
      this.changeDetector.detectChanges();
    });

    let react = this.form.value as AutoReact;
    if (this.isEdit) {
      react.id = this.context.data?.id;
      this.service.replaceReact(react);
    } else {
      this.service.addAutoReact(react);
    }
  }

  public close(): void {
    this.context.completeWith(null);
  }

  public deleteReact(): void {

    this.dialogService.open(new PolymorpheusComponent(ConfirmationDialogComponent, this.injector),
    {
      label: 'Confirm Deletion',
      data: 'Are you sure you want to delete this reaction?',
      size: 's'
    }).subscribe(confirm => {
      if (confirm == true) {
        this.deleting = true;
        this.service.$reacts.pipe(first()).subscribe(() => this.close());
        this.service.deleteReact(this.context.data?.id!);
      }
    });
  }

  public typeChanged(): void {
    if (!this.isAuthor && this.form.controls['author'].validator != null) {
      this.form.controls['author'].clearValidators();
      this.form.controls['author'].setValue('');
    } else if (this.isAuthor) {
      if (this.form.controls['phrase'].validator != null) {
        this.form.controls['phrase'].clearValidators();
      } else {
        this.form.controls['phrase'].setValidators(Validators.required);
      }
    }
  }

}
