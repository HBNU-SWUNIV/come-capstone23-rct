git clone시 터미널에서 `git config --system core.longpaths true` 명령어 입력 후 git clone을 해야 함.


# 한밭대학교 컴퓨터공학과 RCT팀

**팀 구성**
- 20181586 김진우
- 20181612 김자용
- 20181588 김현기

## <u>Teamate</u> Project Background
- ### 필요성
  - 로봇 기술의 발전에 따라 다양한 분야에서의 로봇 산업 발전에 따른 자율화 및 원격제어 로봇 기술 발전의 가속화
  - 현장에서 인간을 대체할 로봇의 역할 증가에 따른 원격 로봇 제어 시스템의 필요성
- ### 기존 해결책의 문제점
  - 로봇의 완전한 자율화의 한계
    - 실제 작업 환경의 불확실성, 변수 통제의 어려움 등 로봇의 완전한 자율화에 한계점 존재
    - 단순 작업이 아닌 위험 현장, 정밀한 조작을 요구하는 작업 현장에서 인간의 개입 없이 로봇이 스스로 판단하고 수행하는 일은 현재 기술로 무리

  - 현재 원격 조종 기술의 문제점
    - 180도 카메라 활용으로 인한 시야각의 한계 → 사각지대 존재로 인한 인식의 어려움
    - 카메라 및 센서 개수의 수 증가 → 사용자에게 직관적이지 않은 조종 환경, 비용과 유지 보수에서 문제 발생
    - 시간지연에 의한 원격조종의 어려움 → 네트워크 지연의 문제 발생

## System Design
  - ### System Requirements
    - 360 device
      - RICOH THETA V
    - WebRTC
    - VR Environment
      - Unity
      - Oculus Quest2
    - Turtlebot3 & OpenManipulatorX
  
  - ### 시스템 구성도
   ![Alt text](image.png)

    
## Case Study
  - ### Description
![image](https://github.com/HBNU-SWUNIV/come-capstone23-rct/assets/93181869/b8107b63-8c46-4dac-8060-81fdc29b52a9)
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Mark1. Mirobot & WebRTC  &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Mark2. Image Processing &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Mark3.Turtlebot3&OpenManipulator
  
## Conclusion
  - ### 인간의 시야와 흡사한 조종 환경을 구사하여 작업자에게 몰입감, 실제와 같은 경험 제공
  - ### 적은 카메라로 보다 많은 영상 데이터 확보 가능 → 사각지대의 한계 개선
  
## Project Outcome
- ### 2023년 한국전기전자학회 학술대회 참가
  ![image](https://github.com/HBNU-SWUNIV/come-capstone23-rct/assets/93181869/abaf5290-3780-4e34-b251-9a80d6b16be7)


- ### 2023년 학부생 시뮬레이션 논문 발표 경진대회 참가(금상, 및 가작상 수상)
  ![상장](https://github.com/HBNU-SWUNIV/come-capstone23-rct/assets/93181869/22a57a9f-6490-49d4-b46a-0deacbfbd138)


- ### WSC2023 참가(12.08~12.14 참가 예정)

