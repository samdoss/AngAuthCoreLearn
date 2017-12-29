
import { Injectable } from '@angular/core';
import { HttpRequest, HttpHandler, HttpEvent, HttpInterceptor, HttpHeaders, HttpResponse, HttpErrorResponse, HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import { LocalStorageService } from 'angular-2-local-storage';
import { LoaderService } from '../../loader/loader.service';

@Injectable()
export class TokenInterceptor implements HttpInterceptor {

    constructor(
        private localStorageService: LocalStorageService,
        public loaderService: LoaderService
    ) { }

    public pendingRequests: number = 0;
    public showLoading: boolean = false;

    intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {

        this.pendingRequests++;
        var token = this.localStorageService.get<string>("accessToken");

        if (token != null) {
            console.log('request.url ', request.url)
            console.log(' s token ', token)
            request = request.clone({
                setHeaders: {
                    Authorization: "Bearer " + token
                }
            });
        }

        return next.handle(request)
            .do(evt => {
                if (evt instanceof HttpResponse) {
                    //console.log('---> status:', evt.status);
                    this.turnOnModal();
                }
            }, (err: any) => {
                this.turnOffModal();
            })
            .finally(() => {
                var timer = Observable.timer(2000);
                timer.subscribe(t => {
                    this.turnOffModal();
                });
            });

    }

    private turnOnModal() {
        if (!this.showLoading) {
            this.showLoading = true;
            this.loaderService.show();
        }
        this.showLoading = true;
    }

    private turnOffModal() {
        this.pendingRequests--;
        if (this.pendingRequests <= 0) {
            if (this.showLoading) {
                this.loaderService.show();
            }
            this.loaderService.hide();
            this.showLoading = false;
        }
    }


}