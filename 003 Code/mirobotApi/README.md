# Mirobot API
1. Anaconda 가상환경 설치 후 python = 3.8 버전으로 가상환경 설치

2. 다음 명령어로 라이브러리 설치
```
pip3 install -r requirements.txt
```

3. mirobotApi\instance\config.py 파일 생성 후 다음 정보 입력 필요
```python
PORTNAME :str = "Your Mirobot PORT" --> 미로봇 시리얼 포트
HOST :str = "Your IP ADDRESS" --> Unity Device의 IP 주소
PORT :int = "Your PORT" --> Unity Device의 포트
```

키보드 실행 명령어
```
python run.py keyboard
```

컨트롤러 실행 명령어
```
python run.py controller
```
