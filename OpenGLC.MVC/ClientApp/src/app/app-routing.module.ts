import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { NewRegisterComponent } from './Pages/Register/new-register.component';
import { LoginComponent } from './Pages/Login/login.component';
import { ForgotPasswordComponent } from './Pages/ForgotPassword/forgot-password.component';


const routes: Routes = [
  {
    path: 'login',
    component: LoginComponent,

  },
  {
    path: 'register',
    component: NewRegisterComponent,

  },
  {
    path: 'passwordRequest',
    component: ForgotPasswordComponent,

  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
