import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { MessagesComponent } from './messages/messages.component';
import { ReactsComponent } from './reacts/reacts.component';
import { ResponsesComponent } from './responses/responses.component';

const routes: Routes = [
  { path: 'responses', component: ResponsesComponent },
  { path: 'reacts', component: ReactsComponent },
  { path: 'messages', component: MessagesComponent },
  { path: '', redirectTo: 'responses', pathMatch: 'full' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
