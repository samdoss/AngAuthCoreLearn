import { Injectable } from '@angular/core';
import { Response, Headers, RequestOptions, Jsonp, Http } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/catch';
import 'rxjs/add/operator/map';

import { Configuration, Constants } from '../../../app.constants';

@Injectable()
export class test_component_1Service {

    constructor(
        private http: Http,
        private Config: Configuration,
        private Cons: Constants

    ) { 
    }
     
    public gettest_component_1List(uid: string, projectHook: string) {
        //let headers = new Headers({ 'Content-Type': 'application/x-www-form-urlencoded' });
        //let options = new RequestOptions({ headers: headers });
        //let objJson = JSON.stringify({ Uid: uid, ProjectHook: projectHook });
        //let apiroot = this.Config.apiKwanJaiURL + 'UnAuthorization/RequestOrder/ListFortest_component_1';
        //return this.http.post(apiroot, objJson, options).map(res => res.json());
    }

     
}