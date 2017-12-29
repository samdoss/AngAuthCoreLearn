import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Router } from '@angular/router';
import { LocalStorageService } from 'angular-2-local-storage';
import { Observable } from 'rxjs';
import { Configuration } from '../../../../app.constants';

import { AuthData } from '../model/auth.data';
import { RegisterData } from '../../register/model/v1/register.data';
import { UserModel } from '../../../models/user-model';

import 'rxjs/add/operator/map'
 
@Injectable()
export class AuthenticationService {

    constructor(
        private _config: Configuration,
        private http: HttpClient,
        private router: Router,
        private localStorageService: LocalStorageService,

    ) { }
    private previousUrl: string = '';
    public userEmail: string;
    sub: any;

    login(username: string, password: string): Observable<AuthData>
    {
        console.log(username, password);
        this.userEmail = username;

        let dataObj = {
            ClientId: this._config.WebBoardClientId,
            UserName: username,
            Password: password,
            GrantType: 'password'
        }

        const url = this._config.apiWebBoardURL + "api/Auth/RequestToken";  //+ "api/v1/Auth";
        let headers = new HttpHeaders({ 'Content-Type': 'text/json' });
        //let options = new RequestOptions({ headers: headers });

        this.sub = this.http.post<AuthData>(url, dataObj, { headers }).catch(this.handleError);

        return this.sub;
    }

    refreshToken(username: string, access_token: string, refresh_token: string): Observable<AuthData> {
        console.log("refreshToken")
        let dataObj = {
            ClientId: this._config.WebBoardClientId,
            Access: access_token,
            Refresh: refresh_token,
            UserName: username,
            GrantType: 'refresh'
        }

        const url = this._config.apiWebBoardURL + "api/v1/Auth";
        let headers = new HttpHeaders({ 'Content-Type': 'text/json' });

        this.sub = this.http.post<AuthData>(url, dataObj, { headers }).catch(this.handleError);

        return this.sub;
    }

    logout(access_token: string, refresh_token: string): Observable<any> {

        let dataObj = {
            Access: access_token,
            Refresh: refresh_token
        }

        const url = this._config.apiWebBoardURL + "api/v1/ManagementTokens/ClearTokens";
        let headers = new HttpHeaders({ 'Content-Type': 'application/json' });

        this.sub = this.http.post(url, dataObj, { headers, responseType: 'text' }).catch(this.handleError);

        // clear token remove user from local storage to log user out
        this.localStorageService.clearAll();

        return this.sub;
    }

    forgetPassword(username: string): Observable<any> {

        this.userEmail = username;

        let dataObj = {
            UserName: username,
            FromUrl: 'WebBoard'
        }

        const url = this._config.apiWebBoardURL + "api/v1/Account/PreResetPassword";
        let headers = new HttpHeaders({
            'Content-Type': 'text/json',
            "Accept": 'application/json'
        });

        this.sub = this.http.post(url, dataObj, { headers }).catch(this.handleError);

        return this.sub;
    }

    checkValidateToken(access_token: string): boolean {

        //Check token (wait authv2 implement)
        return true;
    }

    private handleError(error: any) {
        let errMsg = (error.message) ? error.message :
            error.status ? `${error.status} - ${error.statusText}` : 'Server error';
        console.error(errMsg);
        return Observable.throw(errMsg);
    }

    //private extractDataOb(res: Response) {
    //    let body = res.json() as AuthData;
    //    return body || {};
    //}
    registerUser(user_data: RegisterData): Observable<any> {

        const url = this._config.apiWebBoardURL + "api/V1/Authorization/Register";
        let headers = new Headers({
            'Content-Type': 'application/json'
        });
       

        this.sub = this.http.post(url,  { headers }).catch(this.handleError);

        return this.sub;
    }

    //private extractData(res: Response) {
    //    let body = res.json() as UserModel;
    //    return body || {};
    //}
    getUserData(user_email: string): Observable<UserModel> {
        const url = this._config.apiWebBoardURL + "api/User/getuserdata/?email=" + user_email; // "api/V1/Authorization/" + user_email;
        return this.http.get(url).catch(this.handleError);
    }


    checkRoleForRedirect(tempRoute: string) {
        let Users = this.localStorageService.get<UserModel>("User");
        console.log('User =>> ', Users);
        if (Users == null) {
            this.router.navigate(['/login'], { queryParams: { tempUrl: tempRoute } });
        }
        else {
            console.log('/module_sample1');
            if (tempRoute == '') {
                this.router.navigate(['/module_sample1']);
            }
            else {
                if (this.previousUrl != tempRoute)
                {
                    this.previousUrl = tempRoute
                    this.router.navigate([tempRoute]);
                }
                
            }

        }
    }
} 