## ZMQ 통신 과정
- 기본적으로 유니티에서 파이썬과 ZMQ 통신을 위해 필요한 파일
  - NetMQ.dll
  - AsyncIO.dll
해당 위 파일들은 필수적으로 필요한 파일들이다.

하지만 유니티의 버전에 따라 해당 위 파일 외의 파일이 필요할 수 있다.
해당 외 파일들은 아래 방법을 사용하여 생성한 파일 중에서 NetMQ와 AsyncIO 파일 처럼 드래그앤 드롭으로 들고오면된다.

해당방법은 NetMQ.dll 파일과  AsyncIO.dll 파일을 얻기위한 기본적인 실행방법이다.
- 해당 사이트에서 https://dotnet.microsoft.com/ko-kr/download .NET을 다운받아줘야한다.
- 그리고 빈 폴더에서 터미널을 열고 콘솔을 하나 생성한 후 
- 
```c#
dotnet new console
```
- 실행을 통해 콘솔의 정상 작동을 확인해준다.
```c#
dotnet run 
```
- 정상작동이 확인되었다면 다음으로는 해당 콘솔을 열어두고 밑의 글을 적어두고 터미널에서 
 ```c#
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <TargetFramework>net471</TargetFramework>
  </PropertyGroup>

  <ItemGroup>
    <Reference Include="System" />
    <Reference Include="System.Core" />
    <Reference Include="UnityEngine" />
  </ItemGroup>

  <ItemGroup>
    <PackageReference Include="NetMQ" Version="4.0.1.10" />
  </ItemGroup>

</Project>

```
```c#
dotnet add package NetMQ --version 4.0.1.10
```
을 실행시켜 NetMQ 파일의 패키지를 만들어준다.
- 다음은 마지막 단계로 빌드시켜주는것이다.
- 
```c#
dotnet build
```
해당과정을 거치게 된다면 bin/Debug/net(version) 폴더에 NetMQ.dll,AsyncIO.dll 파일이 존재하게된다.
하지만 유니티 버전에 따라 해당 두개의 파일뿐만이아니라 다른 파일도 필요하게 되는대 해당 파일들도 거의 위 폴더에 존재한다. 버전에 따라 유의하면서 파일을 유니티 Plugins 폴더에 삽입하여 주면된다.

---------------------------------------------------------------------------------------------------
## WebRTC를 통한 카메라 데이터 동기화

- 파이썬과 통신하기위해서는 기본적으로 해당 URL과 스턴서버가 필요하다. 
- WebRTC 통신을 하기위한 기본적인 파일(추후 수정 예정)