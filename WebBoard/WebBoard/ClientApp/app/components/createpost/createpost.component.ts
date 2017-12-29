import { Component, Inject } from '@angular/core';
import { Http } from '@angular/http';

import { Router } from '@angular/router';
@Component({
    selector: 'createpost',
    templateUrl: './createpost.component.html'
})
export class CreatePostComponent {
    public postdata: PostData;

    constructor(public http: Http, @Inject('BASE_URL') public baseUrl: string, public router: Router) {
        this.postdata = new PostData();
    }

    public CreatePost() {
        console.log(this.postdata);
        this.http.post(this.baseUrl + 'api/WebBoardPost/CreatePost', this.postdata).subscribe(result => {
            console.log("Create Success");
            this.router.navigate(['./mainboard']);
        }, error => console.error(error));
    }
}

class PostData {
    postId: number;
    postSubject: string;
    postDetail: string;
    postTimeStamp: Date;
}
