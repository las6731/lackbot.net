import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FilterableContainerComponent } from './filterable-container/filterable-container.component';
import { TuiIslandModule } from '@taiga-ui/kit';
import { TuiLoaderModule } from '@taiga-ui/core';



@NgModule({
  declarations: [
    FilterableContainerComponent
  ],
  imports: [
    CommonModule,
    TuiIslandModule,
    TuiLoaderModule
  ],
  exports: [
    FilterableContainerComponent
  ]
})
export class SharedModule { }
