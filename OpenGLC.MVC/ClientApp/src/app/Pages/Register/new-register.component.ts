import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AccountService } from '../../Services/AccountService';
import { NewRegisterModel } from '../../Models/NewRegisterModel';


@Component({
  selector: 'app-new-register',
  templateUrl: './new-register.component.html',
  styleUrls: ['../Styles/AccountStyles.css']
})
export class NewRegisterComponent implements OnInit {

  constructor(private accountService: AccountService, private router: Router) { }

  newUser: NewRegisterModel = <NewRegisterModel>{}
  passwordConfirm: string = "";
  errorMessage: string = "";
  processing: boolean = false;


  ngOnInit(): void {
    this.newUser.name = "";
    this.newUser.email = "";
  }

  createAccount() {

    this.errorMessage = "";
    this.processing = true;
    if (this.newUser.name.trim() === "" || this.newUser.firstName.trim() === ""
      || this.newUser.userName.trim() === "" || this.newUser.password.trim() === ""
      || this.newUser.email.trim() === "") {
      this.processing = false;
      this.errorMessage = "Todos los campos son obligatorios";
      return;
    }

    if (this.newUser.password !== this.passwordConfirm) {
      this.processing = false;
      this.errorMessage = "La contraseña no coicide con la contraseña de confirmación";
      return;
    }

    this.accountService.register(this.newUser).subscribe({
      next: (data: any) => {
        debugger;
        if (data.passwordHash)
          this.loginRedirect();
      },
      error: (err) => {
        debugger;
        this.errorMessage = err.error.errorMessages[0];
        this.processing = false;
      },
    });

  }

  loginRedirect() {
    this.accountService.login(this.newUser.userName, this.newUser.password).subscribe({
      next: (data: any) => {
        debugger;
        data.userName = this.newUser.userName;
        localStorage.setItem("userData", JSON.stringify(data));
        this.router.navigate(['/']);
      },
      error: (err) => {
        debugger;
        alert("Ha ocurrido un error -->" + err.error.errorMessages[0]);
      },
    });
  }
}
