# StickMan_Hack-n_Slash
 
 dùng các nút arrow để di chuyển
 Q để tấn công
 Space để nhảy
 khi đang trên tường thì space lần nx để wallJump


 # Logic StateMachine
     
    PartrolState: trạng thái suy nghĩ 
    IdleState : trạng thái đứng yên
    CastState : gọi sấm chớp
    AtkState trạng thái tấn công

    Kịch bản:
        Trạng thái bắt đầu : IdleState (0)
            Hành động : StopMoving
                        Đứng im ngẫu nhiên 1-2 
                        Hết thời gian chuyển sang PartrolState(1)

        Trạng thái Tiếp theo  : PartrolState
            Hành Động : Suy nghĩ trong khoảng thời gian 1-2s
                        Nếu phát hiện ng chơi :
                            + trong tầm đánh -> AtkState(2)
                            + ngoài tầm đánh thì đuổi theo
                        Nếu ko phát hiện :
                            + nếu trong thòi gian suy nghĩ thì sẽ di chuyển xung quanh
                            + hết thòi gian -> IdleState

        Trạng thái Tiếp theo : AtkState
            Hành động : StopMoving , tấn công về phía ng chơi 
                        Nếu mục tiêu vẫn trong tầm nhìn : sau 1s -> CastState(3)
                        Nếu mục tiêu ngoài tầm nhìn :
                            trong 3s -> IdleSate
                            sau 3s ->  PartrolState
        Trạng thái Tiếp theo : CastState 
            Hành động : triệu hồi sấm sét đánh xuống 
                        sau 3s:
                            + Nếu phát hiện ng chơi -> AtkState
                            + Nếu ko phát hiện -> PatrolState

Note* : kiểm tra tầm nhìn và tầm đánh
        hàm moving
        từ AtkState -> IdleSate, PartrolState
