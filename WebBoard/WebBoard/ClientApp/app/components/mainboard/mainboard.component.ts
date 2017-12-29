import { Component, Inject } from '@angular/core';
import { Http } from '@angular/http';
import { Router } from '@angular/router';
@Component({
    selector: 'mainboard',
    templateUrl: './mainboard.component.html'
})
export class MainBoardComponent {
    public postdata: PostData[];

    constructor(http: Http, @Inject('BASE_URL') baseUrl: string , public router : Router) {
        http.get(baseUrl + 'api/WebBoardPost/GetPostList').subscribe(result => {
            console.log('result', result);
            
            this.postdata = result.json() as PostData[];
            console.log('postdata', this.postdata);
        }, error => console.error(error));
    }


    public ViewPost(postId: number)
    {
        localStorage.setItem("postId", postId.toString());
        this.router.navigate(['./viewpost']);
    }

}

interface PostData {
    postId: number;
    postSubject: string;
    postDetail: string;
    postTimeStamp: Date;
}