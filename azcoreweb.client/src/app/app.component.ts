import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

interface Status {
  info: string;
}

interface WeatherForecast {
  date: string;
  temperatureC: number;
  temperatureF: number;
  summary: string;
}

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  standalone: false,
  styleUrl: './app.component.css'
})
export class AppComponent implements OnInit {
  public forecasts: WeatherForecast[] = [];
  public status: Status = { info: '' };

  constructor(private http: HttpClient) {}

  ngOnInit() {
    this.getInfo();
    this.getForecasts();
  }

  getForecasts() {
    this.http.get<WeatherForecast[]>('/weatherforecast').subscribe(
      (result) => {
        this.forecasts = result;
      },
      (error) => {
        console.error(error);
      }
    );
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
