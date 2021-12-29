import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

const routes: Routes = [
  { path: 'responses', loadChildren: () => import('./responses/responses.module').then(m => m.ResponsesModule) },
  { path: 'reacts', loadChildren: () => import('./reacts/reacts.module').then(m => m.ReactsModule) },
  { path: 'messages', loadChildren: () => import('./messages/messages.module').then(m => m.MessagesModule) },
  { path: '', redirectTo: 'responses', pathMatch: 'full' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
