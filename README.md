# Description

Csts is a translator of c# classes to Typescript interfaces.

## Usage

Add the CstsHelpers dependency to your project. After that, compile Csts and add the path to your PATH variable.

Add the `[TypeScriptModel]` attribute to the classes that should be transilirons.

```c#
[TypeScriptModel]
public class WeatherForecast
{
    public DateTime Date { get; set; }
    public int TemperatureC { get; set; }
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
    public string Summary { get; set; }
}
```

Create a `cstsSettings.json` file at the root of the project.

```js
{
    "BuildPath": "ClientApp/src/"
}
```

Use csts in the console. The result will be the generated csts.ts file.
 
```ts
export interface WeatherForecast{
    date: Date;
    temperatureC: number;
    temperatureF: number;
    summary: string;
}
```

Use it

![example](imgs/img1.jpg?raw=true "Title")
