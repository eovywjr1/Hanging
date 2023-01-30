## Hanging
  프로젝트의 이름은 Hanging으로, 교수형을 의미합니다.

## 목차
  1. 게임 소개
  2. 팀원 소개
  3. 기술 스택
  4. 구현 기능
  5. 배운 점
  
## 팀원 소개
  - 팀장 : 박주현(디자인)
  - 팀원 : 조민수(클라이언트)
  - 팀원 : 조유민(클라이언트)
  - 팀원 : 정성희(클라이언트, 아트)
  - 팀원 : 김세민(아트)
  
## 게임 소개
  큰 전쟁이 끝난 직후, 세계는 통합되어 정부군과 반란군으로 나뉩니다. <br/>
  정부는 부족한 사람 수를 충족하기 위해 복제인간을 제작했고, 플레이어는 그 결과 탄생한 복제인간입니다.<br/>
  플레이어는 구직을 통해 사형감별사 직업을 얻어 돈을 벌고, 정부군 또는 반란군을 도와 살아남는 게임입니다.

## 기술 스택
  - 게임 엔진 : Unity (2021.3.12f1 LTS)
  - 프로그래밍 언어 : C#

## 구현 기능
  - 제한 시간 설정
  
  - 사형 판결    
    - 사형 : 사형수를 마우스 드래그로 임계선 위로 올림
    - 생존 : 마우스로 밧줄을 가로질러서 자르는 행위
    - 사형 판결 후 씬 전환
  
  - 사건기록서
    - 사형수를 클릭하면 사건기록서 나타남
    - 사형수 정보 랜덤 생성
    - 이름을 클릭하면 피해자 정보 나타남
    - 사건기록서를 마우스로 이동
  
  - 카메라 이동
    - 스페이스 바를 누르면 카메라 이동

## 배운 점
  - IsPointerOverGameObejct 사용
    - 콜라이더를 사용하여 마우스 클릭을 사용하였는데, 오브젝트가 겹치면 의도한 오브젝트 클릭이 안된 것을 해결했습니다.
    
  - 기획서의 중요성
    - 담당하는 모든 인원이 똑같은 방향으로 갈 수 있게 작성해야 하고, 여러 수정본을 만드는 것보다는 페이지 하나를 계속 수정해나가는 것이 비교적 좋다는 것을 알았습니다.
    
  - 피드백을 통해 근본적인 시스템부터 구하는 것이 좋다는 것을 알았습니다.
    - 게임 흐름대로 구현하면 원하는 기능들이 계속 추가되는 것을 깨달았습니다.
    - 시스템부터 구현한 후 부가 기능을 만들어가는 방식으로 작업했습니다.
