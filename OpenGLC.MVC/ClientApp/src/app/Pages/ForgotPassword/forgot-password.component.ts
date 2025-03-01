
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AccountService } from '../../Services/AccountService';

@Component({
  selector: 'forgot-component',
  templateUrl: './forgot-password.component.html',
  styleUrls: ['../Styles/AccountStyles.css']
})
export class ForgotPasswordComponent implements OnInit {

  constructor(private accountService: AccountService, private router: Router) { }

  userNameOrEmail: string = "";
  processing: boolean = false;
  responseMessage : string = "";
  ngOnInit(): void {
    debugger;
  }

  sendPasswordRequest(){
    this.processing =  true;
    //AJAX call to server
    this.accountService.sendPasswordRequest(this.userNameOrEmail)
    .subscribe({next: (data: any) =>{
        this.processing =  false;
        this.responseMessage = "Hemos procesado tu solicitud, revisa tus correos electrónicos para restablecer tu contraseña"

    }, error: (err)=>{
        alert("HUBO UN ERROR, INTENTALO MÁS TARDE");
        this.processing =  false;
    }})

  }

  
  
}
