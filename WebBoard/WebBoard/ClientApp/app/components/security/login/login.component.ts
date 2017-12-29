import { Component, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Response, Headers, RequestOptions, Http } from '@angular/http';
import 'rxjs/add/observable/of';
import { Observable } from 'rxjs/Rx';
import { Subscription } from "rxjs/Subscription";
import { Title } from '@angular/platform-browser';
import { LocalStorageService } from 'angular-2-local-storage';

import { Configuration, Constants } from '../../../app.constants';
//import { validate } from '../../../sharedservice/validate';

/* Import service */
import { AuthenticationService } from '../authentication/service/auth.service';
/* Import model */
import { AuthData } from '../authentication/model/auth.data';
import { UserModel } from '../../models/user-model';

/* Import Modal */
import { RegisterComponent } from '../register/register.component';
import { AuthenModalComponent } from '../authen-modal/authen-modal.component';


@Component({
    selector: 'login',
    templateUrl: './login.html',
    //styleUrls: ['./login.component.css']

})

export class LoginComponent implements OnInit {
    constructor(
        private AuthService: AuthenticationService,
        private localStorageService: LocalStorageService,
        private titleService: Title,
        private router: Router,
        private Config: Configuration,
        private Cons: Constants,
        //private http: Http,

    ) {
        this.titleService.setTitle("WebBoard");
    }

    @ViewChild('RegisterModal') RegisterModal: RegisterComponent;
    @ViewChild('AfterAuthenModal') AfterAuthenModal: AuthenModalComponent;

    ngOnInit(): void {

        //Check token and redirect to Home
        if (this.localStorageService.get("accessToken")) {
            //this.router.navigate(['/crm']);
            this.AuthService.checkRoleForRedirect("/login");
        }

    }
    private username: string = "123123123";
    private password: string = "123123123";
    //peerapol@builk.com builk1123

    //"userName": "csc@xxx.xxx",
    //"password": "WebBoard123",
    auth_data: AuthData;

    webLogin() {
        //this.startLoadChat();
        //this.setDummyData();
    }

    //listmanager() {
    //    this.router.navigateByUrl('/listmanager');
    //}

    //module_sample1() {
    //    this.router.navigateByUrl('/module_sample1');
    //}

    //management() {
    //    this.router.navigateByUrl('/management');
    //}

    //crm() {
    //    let headers = new Headers({ 'Content-Type': 'application/x-www-form-urlencoded' });
    //    let options = new RequestOptions({ headers: headers });
    //    let objData = { OrgGuId: "x5" };
    //    let objJson: string = JSON.stringify(objData);
    //    return this.http.post(/*apiroot*/'http://localhost:50917/api/V1/ProjectController/ListFormodule_sample1', objJson, options)
    //        .map(res => res.json())
    //        .subscribe(data => { console.log(' x ', data) }, error => { console.log(' xerror ', error) });

    //    //this.router.navigateByUrl('/crm');
    //}

    onLogin() {
        this.Login(this.username, this.password);
    }

    onLogout() {
        var access = this.localStorageService.get<string>("accessToken");
        var refresh = this.localStorageService.get<string>("refreshToken");
        this.AuthService.logout(access, refresh);
    }

    Login(_username: string, _password: string) {
        //console.log(_username, _password);
        this.AuthService.login(_username, _password)
            .subscribe( (response ) => { 
                this.auth_data = response; 
                //Storage Session
                this.localStorageService.set("userEmail", _username);
                this.localStorageService.set("accessToken", this.auth_data.access_token);
                this.localStorageService.set("refreshToken", this.auth_data.refresh_token);
                this.localStorageService.set("expiresTime", (Date.now() / 1000) + this.auth_data.expires_in);
                //console.log(' get accessToken  ', this.localStorageService.get("accessToken"));
                this.getUserData();
            }, error => {
                console.log("login error", error);
            });
    }

    getUserData() {
        this.AuthService.getUserData(this.username)
            .subscribe((user: UserModel) => {
                //console.log('qwe check  ', user);
                //console.log('check => localStorage Users', this.localStorageService.get<UserModel>("User"));

                this.localStorageService.set("User", user);//real
                //this.localStorageService.set("User", this.Cons.userData); //dummy
                let Users = this.localStorageService.get<UserModel>("User");

                //console.log('set => localStorage Users', Users);
                this.AuthService.checkRoleForRedirect('');
            }, error => {
                //this.router.navigateByUrl('/module_sample1');
               // console.log("Get User Data error", error);
            })
    }

    register() {
        this.RegisterModal.show();
    }

    //private subscrip: Subscription;
    //private stopLoadChat() {
    //    if (!validate.isUndefinedNullOrEmpty(this.subscrip)) {
    //        this.subscrip.unsubscribe();
    //    }
    //}
    //private startLoadChat() {
    //    this.subscrip = Observable.timer(3000, 3000).subscribe((t: any) => { this.LoadChatDataList(); });
    //}

    //LoadChatDataList() {
    //    this.stacked = this.stacked + 1;
    //}
}