import { Inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { NewRegisterModel } from '../Models/NewRegisterModel';

@Injectable({
  providedIn: 'root'
})
export class AccountService {

  constructor(private httpClient: HttpClient, @Inject('BASE_URL')  private baseUrl: string) { }

  login(userName: string, password: string, generateDeleteToken: boolean = false) {
    debugger;
    var requestQry = "Account/login?userName=" + userName + "&password=" + password;
    if (generateDeleteToken === true) {
      requestQry += "&tokenForDeleteAction=true"
    }
    // tokenForDeleteAction=false
    return this.httpClient.get(this.baseUrl + requestQry, { withCredentials: true })
  }

  register(newUser: NewRegisterModel) {
    debugger;
    return this.httpClient.post(this.baseUrl + "api/Account/register", newUser);
  }

  token: string = "";

  isUserLogged(): boolean {
    return false;
  }

  getUserData = () => {
    let data = localStorage.getItem("userData");
    if (data !== null)
      return JSON.parse(data);
    else
      return false;
  }

  isNullToken(): boolean {
    return this.token === "";
  }

}
