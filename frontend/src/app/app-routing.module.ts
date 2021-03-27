import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { MessagesComponent } from './messages/messages.component';
import { ReactsComponent } from './reacts/reacts.component';
import { ResponsesComponent } from './responses/responses.component';

const routes: Routes = [
    { path: '', redirectTo: 'responses', pathMatch: 'full' },
    { path: 'responses', component: ResponsesComponent },
    { path: 'reacts', component: ReactsComponent },
    { path: 'messages', component: MessagesComponent }
];

@NgModule({
    imports: [RouterModule.forRoot(routes)],
    exports: [RouterModule]
})
export class AppRoutingModule { }
