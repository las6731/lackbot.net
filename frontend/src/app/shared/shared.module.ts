import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FilterableContainerComponent } from './filterable-container/filterable-container.component';
import { TuiIslandModule } from '@taiga-ui/kit';



@NgModule({
  declarations: [
    FilterableContainerComponent
  ],
  imports: [
    CommonModule,
    TuiIslandModule
  ],
  exports: [
    FilterableContainerComponent
  ]
})
export class SharedModule { }
