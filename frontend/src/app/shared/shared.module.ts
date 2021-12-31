import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FilterableContainerComponent } from './components/filterable-container/filterable-container.component';
import { TuiIslandModule } from '@taiga-ui/kit';
import { TuiButtonModule, TuiLoaderModule } from '@taiga-ui/core';
import { ConfirmationDialogComponent } from './components/confirmation-dialog/confirmation-dialog.component';



@NgModule({
  declarations: [
    FilterableContainerComponent,
    ConfirmationDialogComponent
  ],
  imports: [
    CommonModule,
    TuiIslandModule,
    TuiLoaderModule,
    TuiButtonModule
  ],
  exports: [
    FilterableContainerComponent
  ]
})
export class SharedModule { }
