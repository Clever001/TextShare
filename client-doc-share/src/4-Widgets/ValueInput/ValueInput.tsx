import "./ValueInput.css"

type Props = {
    type: "textarea" | "select" | "input" | "checkbox",
    keyPosition: "left" | "right",
    label: string,
    formSearchName: string,
    hint?: string | null,
    possibleSelections?: SelectionInfo[],
    defaultValue?: string | null,
}

export type SelectionInfo = {
    htmlValue: string,
    presentValue: string,
}

export default function ValueInput({
    type, keyPosition, label, formSearchName, hint = null, 
    possibleSelections = [], defaultValue = null
}: Props) {
    return (
        <div className="value-input"
            style={(type == "textarea") ? {height: "63px"} : {}}
            >
            <p className="key">{label}</p>
            {type == "textarea" &&
                <textarea  
                    style={{height: "63px"}}
                    className="value"
                    name={formSearchName} id={formSearchName} placeholder={hint} value={defaultValue}/>
            }
            {type == "select" &&
                <select
                name={formSearchName} id={formSearchName}>
                    {possibleSelections.map(s => {
                        return <option value={s.htmlValue}>{s.presentValue}</option>
                    })}
                </select>
            }
            {type == "input" &&
                <input className="value"
                    type="text" name={formSearchName} id={formSearchName} placeholder={hint} value={defaultValue} />
            }
            {type == "checkbox" &&
                <input 
                    style={{width: "20px", height: "20px", alignItems: "start", display: "flex"}}
                    className="value"
                    type="checkbox" name={formSearchName} id={formSearchName} placeholder={hint} value={defaultValue} />
            }
        </div>
    )
}