import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ReactsRoutingModule } from './reacts-routing.module';
import { ReactsComponent } from './reacts.component';


@NgModule({
  declarations: [
    ReactsComponent
  ],
  imports: [
    CommonModule,
    ReactsRoutingModule
  ]
})
export class ReactsModule { }
