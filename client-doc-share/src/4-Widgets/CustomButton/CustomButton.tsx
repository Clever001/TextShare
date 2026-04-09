type Props = {
    leftIconUrl: string | null,
    rightIconUrl: string | null,
    text: string | null,
    color: "blue" | "green" | "yellow" | "red",
    type: "submit" | "button",
    target: string,
    onClick: () => void
}

export default function CustomButton({
    leftIconUrl = "/img/add_circle.svg",
    rightIconUrl = "/img/add_circle.svg",
    text = "Нажми на меня!",
    color = "blue",
    type = "button",
    target,
    onClick = () => {},
}: Props) {
    return (
        <button type={type} className="custom-button" onClick={onClick}>
            {leftIconUrl && 
                <img src={leftIconUrl} alt={target} />
            }
            {text && text}
            {rightIconUrl && 
                <img src={rightIconUrl} alt={target} />
            }
        </button>
    )
}