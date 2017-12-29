import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Configuration } from "../../../app.constants";
import { LocalStorageService } from 'angular-2-local-storage';
import { Observable } from "rxjs/Observable";
import { AuthenticationService } from "../../security/authentication/service/auth.service";


@Component({
    styles: [``],
    templateUrl: './menu.html'
})

export class MenuComponent implements OnInit {

    constructor(
        private localStorageService: LocalStorageService,
        private _config: Configuration,
        private http: HttpClient,
        private router: Router,
        private AuthService: AuthenticationService,
    ) {
        console.log('menu');
    }
    errTxt: string = "";
    ngOnInit() {


    }

    LearPost() {
        this.ppPost().subscribe(
            Response => {
                console.log("post success")
                console.log(Response);
            },
            err => {
                //unauthorized refresh token 
                console.log("unauth")
                console.log("err", err)
                this.errTxt = err;
                //if (err.status == 401) { 
                //            //re function
                //            this.LearPost(); 
                //}
            }
        );
    }

    ppPost(): Observable<string> {

        let url = this._config.apiWebBoardURL + "api/ApiTest/www";

        let headers = new HttpHeaders({ 'Content-Type': 'application/json' });

        let thsSub = this.http.post<string>(url, { value: "s1" }, { headers })
        return thsSub;

    }



    menu() {
        this.router.navigateByUrl('/module_sample1/menu');
    }
    test_component_1() {
        this.router.navigateByUrl('/module_sample1/test_component_1');
    }
    test_component_1csc() {
        this.router.navigateByUrl('/module_sample1/test_component_2');
    }

}
