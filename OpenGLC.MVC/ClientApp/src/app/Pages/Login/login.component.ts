
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { environment } from 'src/environments/environment';
import { AccountService } from '../../Services/AccountService';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['../Styles/AccountStyles.css']
})
export class LoginComponent implements OnInit {

  constructor(private accountService: AccountService, private router: Router) { }

  userName: string = "";
  password: string = "";
  processing: boolean = false;
  environmentMessage = "";

  ngOnInit(): void {
 
      this.environmentMessage = ""

    debugger;
    if (this.accountService.getUserData() !== null)
      this.router.navigate(['/']);

  }

  login() {
    debugger;
    this.processing = true;
    this.accountService.login(this.userName, this.password).subscribe({
      next: (data: any) => {
        debugger;
        data.userName = this.userName;
        localStorage.setItem("userData", JSON.stringify(data));
        this.processing = false;
        this.router.navigate(['/']);
      },
      error: (err) => {
        debugger;
        alert("Contraseña incorrecta o servicio no disponible! -->" + err.error.errorMessages[0]);
        this.processing = false;
      },
    });

  }

  goToRegister() {
    this.router.navigate(['/register']);
  }

}
