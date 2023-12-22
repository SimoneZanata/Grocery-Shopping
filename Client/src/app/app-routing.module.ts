import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './components/login/login.component';
import { RegisterComponent } from './components/register/register.component';
import { AuthGuard } from './guards/auth-guard';
import { ListItemsComponent } from './components/list-items/list-items.component';
import { AddItemComponent } from './components/add-item/add-item.component';
import { MainPageComponent } from './components/main-page/main-page.component';
import { EditItemComponent } from './components/edit-item/edit-item.component';
import { LogoutComponent } from './components/logout/logout.component';

const routes: Routes = [
  {
    path: "",
    component: MainPageComponent,
    canActivate: [AuthGuard],
    children: [
      { path: "items", component: ListItemsComponent },
      { path: "edit/:itemId", component: EditItemComponent },
      { path: "add", component: AddItemComponent},
      { path: "logout", component: LogoutComponent},
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
