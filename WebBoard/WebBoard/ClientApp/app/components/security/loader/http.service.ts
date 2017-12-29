import { LoaderService } from './loader.service';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';
import 'rxjs/add/operator/do';
import 'rxjs/add/operator/finally';
import { Http, XHRBackend, RequestOptions, Request, RequestOptionsArgs, Response, Headers } from '@angular/http';
 
@Injectable()
export class HttpService extends Http {
    public pendingRequests: number = 0;
    public showLoading: boolean = false;

    constructor(
        backend: XHRBackend,
        defaultOptions: RequestOptions,
        public loaderService: LoaderService
    ) {
        super(backend, defaultOptions);
    }

    request(url: string | Request, options?: RequestOptionsArgs): Observable<Response> {
        return this.intercept(super.request(url, this.getRequestOptionArgs(options)));
    }

    get(url: string, options?: RequestOptionsArgs): Observable<Response> {
        return this.intercept(super.get(url, this.getRequestOptionArgs(options)));
    }

    post(url: string, body: string, options?: RequestOptionsArgs): Observable<Response> {
        return this.intercept(super.post(url, body, this.getRequestOptionArgs(options)));
    }

    put(url: string, body: string, options?: RequestOptionsArgs): Observable<Response> {
        return this.intercept(super.put(url, body, this.getRequestOptionArgs(options)));
    }

    delete(url: string, options?: RequestOptionsArgs): Observable<Response> {
        return this.intercept(super.delete(url, this.getRequestOptionArgs(options)));
    }

    getRequestOptionArgs(options?: RequestOptionsArgs): RequestOptionsArgs {

        var token = localStorage.getItem('WebBoard.accessToken');

        if (token) {

            if (options) {
                if (!options.headers!.get('Authorization')) {
                    token = token.replace(/"/g, "");
                    //console.log("option");
                    options.headers!.append('Authorization', "Bearer ".concat(token));
                    return options;
                } else {
                    return options;
                }
            } else {
                token = token.replace(/"/g, "");
                //console.log("non-option");
                var headers = new Headers({ 'Authorization': "Bearer ".concat(token) });
                //console.log(headers);
                var optionsAuth = new RequestOptions({ headers: headers });
                return optionsAuth;
            }
        } else {
            return options!;
        }

    }

    intercept(observable: Observable<Response>): Observable<Response> {
        //console.log("In the intercept routine..");
        this.pendingRequests++;
        return observable
            .catch(this.onCatch)
            .do((res: Response) => {
                //console.log("Response: " + res);
                //this.turnOnModal();
            }, (err: any) => {
                //this.turnOffModal();
                //console.log("Caught error: " + err);
            })
            .finally(() => {
                //console.log("Finally.. delaying, though.")
                //var timer = Observable.timer(1000);
                //timer.subscribe(t => {
                //    this.turnOffModal();
                //});
            });
    }

    private onCatch(error: any, caught: Observable<any>): Observable<any> {
        return Observable.throw(error);
    }

    private turnOnModal() {
        if (!this.showLoading) {
            this.showLoading = true;
            //$('body').spin("modal", "#FFFFFF", "rgba(51, 51, 51, 0.1)");
            //this.loaderService.show();
            //console.log("Turned on modal");
        }
        this.showLoading = true;
    }

    private turnOffModal() {
        this.pendingRequests--;
        if (this.pendingRequests <= 0) {
            if (this.showLoading) {
                //$('body').spin("modal", "#FFFFFF", "rgba(51, 51, 51, 0.1)");
                //this.loaderService.show();
            }
            //this.loaderService.hide();
            this.showLoading = false;
        }
        //console.log("Turned off modal");
    }
}