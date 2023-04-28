import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import {PageNotFoundComponent} from "./pagenotfound/component/page-not-found/page-not-found.component";

const routes: Routes = [
  {
    path: 'webhook',
    loadChildren: () => import('./webhook/webhook.module').then(x => x.WebhookModule),
  },
  {
    path: '',
    redirectTo: 'webhook',
    pathMatch: 'full'
  },
  {
    path: '**',
    component: PageNotFoundComponent
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
