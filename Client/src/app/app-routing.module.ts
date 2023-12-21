import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './components/login/login.component';
import { RegisterComponent } from './components/register/register.component';
import { AuthGuard } from './guards/auth-guard';
import { ListItemsComponent } from './components/list-items/list-items.component';

const routes: Routes = [
  {
    path: "",
    component: ListItemsComponent,
    canActivate: [AuthGuard],
    children: [
      { path: "items", component: ListItemsComponent },
      { path: "", redirectTo: "items", pathMatch: 'full' },
    ],
  },
  {
    path: "login",
    component: LoginComponent,
  },
  {
    path: "register",
    component: RegisterComponent,
  },
  { path: "**", redirectTo: "" },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
