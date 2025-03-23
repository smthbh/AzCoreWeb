import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

interface Status {
  info: string;
}

interface Response {
  info: string;
}

interface Account {
  username: string;
  password: string;
}

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  standalone: false,
  styleUrl: './app.component.css'
})
export class AppComponent implements OnInit {
  public status: Status = { info: '' };
  public Response: Response = { info: '' };
  username: string = '';
  password: string = '';

  constructor(private http: HttpClient) {}

  ngOnInit() {
    this.getInfo();
  }

  getInfo() {
    this.http.get<Status>('/status').subscribe(
      (result) => {
        this.status.info = result.info;
      },
      (error) => {
        console.error(error);
      }
    );
  }

  createAccount(username: string, password: string) {
    const account: Account = { username, password };
    this.http.post<Response>('/Account/Create', account).subscribe(
      (result) => {
        this.Response.info = result.info;
      },
      (error) => {
        console.error(error);
        this.Response.info = error.error.title ?? 'An error occured';
      }
    );
  }

  onCreateClick(username: string, password: string): string {
    this.createAccount(username, password);
    this.username = '';
    this.password = '';
    return this.Response.info;
  }

  get formattedStatusInfo(): string {
    return this.status.info.replace(/\r\n/g, '<br>');
  }

  title = 'azcoreweb.client';
}
