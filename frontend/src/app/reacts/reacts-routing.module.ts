import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ReactsComponent } from './reacts.component';

const routes: Routes = [{ path: '', component: ReactsComponent }];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ReactsRoutingModule { }
