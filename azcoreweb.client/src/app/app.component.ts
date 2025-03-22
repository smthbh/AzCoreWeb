import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

interface Status {
  info: string;
}

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  standalone: false,
  styleUrl: './app.component.css'
})
export class AppComponent implements OnInit {
  public status: Status = { info: '' };

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

  get formattedStatusInfo(): string {
    return this.status.info.replace(/\r\n/g, '<br>');
  }

  title = 'azcoreweb.client';
}
