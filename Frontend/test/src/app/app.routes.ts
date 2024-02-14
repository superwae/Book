import { RouterModule, Routes } from '@angular/router';
import { SignUpComponent } from './sign-up/sign-up.component';
import { LoginComponent } from './login/login.component';
import { NgModule } from '@angular/core'; 


export const routes: Routes = [

    { path:'',pathMatch:'full',redirectTo:'register'},
    { path: 'login', component: LoginComponent },
    { path: 'register', component: SignUpComponent },
  
];
@NgModule({
    imports:[RouterModule.forRoot(routes)],
    exports:[RouterModule]
  })
  export class AppRoutingModule{}
